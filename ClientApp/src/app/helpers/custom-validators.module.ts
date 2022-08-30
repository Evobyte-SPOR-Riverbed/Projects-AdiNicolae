import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, ValidatorFn } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class CustomValidatorsModule {
  static mustMatch(controlName: string, checkControlName: string): ValidatorFn {
    return (controls: AbstractControl) => {
      const control = controls.get(controlName);
      const checkControl = controls.get(checkControlName);
      if (checkControl?.errors && !checkControl.errors['mismatch']) {
        return null;
      }

      const isValid = control?.value === checkControl?.value;
      controls.get(checkControlName)?.setErrors(isValid ? null : { 'mismatch': true });
      return isValid ? null : { 'mismatch': true };
    };
  }

  static noWhitespace(control: FormControl) {
    const isWhitespace = (control.value as string).indexOf(' ') >= 0;
    const isValid = !isWhitespace;
    return isValid ? null : { 'whitespace': true };
  }
}
