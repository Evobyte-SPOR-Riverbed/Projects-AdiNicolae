import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class CustomValidatorsModule {
  static noWhitespace(controlName: string): ValidatorFn {
    return (c: AbstractControl): ValidationErrors | null => {
      const control = c.get(controlName);
      if (control?.errors && !control?.errors['whitespace']) {
        return null;
      }

      const isWhitespace = control?.value.indexOf(' ') > -1;
      c.get(controlName)?.setErrors(isWhitespace ? { 'whitespace': true } : null);
      return isWhitespace ? { 'whitespace': true } : null;
    };
  }

  static mustMatch(controlName: string, checkControlName: string): ValidatorFn {
    return (c: AbstractControl): ValidationErrors | null => {
      const control = c.get(controlName);
      const checkControl = c.get(checkControlName);
      if (checkControl?.errors && !checkControl.errors['mismatch']) {
        return null;
      }

      const isValid = control?.value === checkControl?.value;
      c.get(checkControlName)?.setErrors(isValid ? null : { 'mismatch': true });
      return isValid ? null : { 'mismatch': true };
    };
  }
}
