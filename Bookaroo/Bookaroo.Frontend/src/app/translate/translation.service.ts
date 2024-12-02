import { Injectable } from '@angular/core';
import * as enlocale from './en.json';
import * as sllocale from './sl.json';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private translations: any = {};

  constructor() { }

  loadTranslations(locale: string): void {
    this.translations = {};

    switch (locale) {
      case 'en':
        this.translations = enlocale;
        break;
      case 'sl':
        this.translations = sllocale;
        break;
      default:
        this.translations = enlocale;
        break;
    }

  }

  translate(key: string): string {
    return this.translations[key] || key;
  }
}