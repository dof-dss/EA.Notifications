import { Component } from '@angular/core';
import { SwPush } from '@angular/service-worker';
import { API } from 'aws-amplify';
import { from } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ea-notifications-pwa';
  showError = false;
  showSuccess = false;
  errorMessage: any;

  constructor(private swPush: SwPush) {}

  subscribeToNotifications() {
    console.log(environment.VAPID_PUBLIC_KEY);
    this.swPush
      .requestSubscription({
        serverPublicKey: environment.VAPID_PUBLIC_KEY
      })
      .then(sub => {
        this.addPushSubscriber(sub)
          .pipe(finalize(() => this.showSuccess = true));
      })
      .catch(err => {
        console.error('Could not subscribe to notifications', err);
        this.errorMessage = err;
        this.showError = true;
      }
        );
  }

  addPushSubscriber(sub: PushSubscription) {
    return from(API.post('ea-notifications', '/push/subscribe', {
      body: sub
    }));
  }
}
