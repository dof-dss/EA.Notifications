import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import Amplify, {API} from 'aws-amplify';
import awsconfig from './aws-exports';

if (environment.production) {
  enableProdMode();
}

Amplify.configure(awsconfig);

Amplify.configure({
  API: {
      endpoints: [
          {
              name: 'ea-notifications',
              endpoint: 'https://9aaqg0xo1k.execute-api.eu-west-2.amazonaws.com/beta'
          }
      ]
  }
});

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
