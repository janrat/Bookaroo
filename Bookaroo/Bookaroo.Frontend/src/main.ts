import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';
import { AppComponent } from './app/app.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './app/auth/auth.interceptor';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { LOCALE_ID, PLATFORM_ID } from '@angular/core';
import { TranslationService } from './app/translate/translation.service';
import { provideHttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { LanguageService } from './app/translate/language.service';

let locale = 'en';
if (isPlatformBrowser(PLATFORM_ID)) {
  locale = localStorage.getItem('locale') || 'en';
}

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    { provide: LOCALE_ID, useValue: locale },
    LanguageService,
    TranslationService,
    { provide: PLATFORM_ID, useValue: 'browser' }
  ]
}).catch((err) => console.error(err));
