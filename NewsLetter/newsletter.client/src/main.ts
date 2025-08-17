import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import 'zone.js'; // Importing zone.js for Angular's change detection and async operations
import { provideRouter } from '@angular/router';

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
