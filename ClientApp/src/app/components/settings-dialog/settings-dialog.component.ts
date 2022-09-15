import { formatDate } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTabGroup } from '@angular/material/tabs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomValidatorsModule } from '../../helpers/custom-validators.module';
import { DrinkerType, SexType } from '../../helpers/enums.module';
import { AuthenticationService } from '../../services/authentication.service';
import { CountryService, ICountry } from '../../services/country.service';
import { UserService } from '../../services/user.service';

interface Drinker {
  value: number | DrinkerType;
  viewValue: string;
}

interface Sex {
  value: number | SexType;
  viewValue: string;
}

@Component({
  selector: 'app-settings-dialog',
  templateUrl: './settings-dialog.component.html',
  styleUrls: ['./settings-dialog.component.css']
})
export class SettingsDialogComponent {
  // Validating messages
  formControlsValidations = {
    'firstName': [
      { type: 'required', message: 'First name is required.' }
    ],
    'lastName': [
      { type: 'required', message: 'Last name is required.' }
    ],
    'emailAddress': [
      { type: 'required', message: 'Email address is required.' },
      { type: 'email', message: 'Email address is not valid.' }
    ],
    'password': [
      { type: 'required', message: 'Password is required.' },
      { type: 'minlength', message: 'Password must be at least 14 characters long.' },
      { type: 'whitespace', message: 'Password doesn\'t accept whitespaces.' }
    ],
    'confirmPassword': [
      { type: 'required', message: 'Confirmation password is required.' },
      { type: 'minlength', message: 'Confirmation password must be at least 14 characters long.' },
      { type: 'mismatch', message: 'Confirmation password does not match.' }
    ],
    'birthday': [
      { type: 'required', message: 'Birthday is required.' }
    ],
    'country': [
      { type: 'required', message: 'Country is required.' }
    ],
    'sex': [
      { type: 'required', message: 'Sex is required.' }
    ],
    'drinkerType': [
      { type: 'required', message: 'Drinker type is required.' }
    ],
    'actualPassword': [
      { type: 'required', message: 'Confirmation password is required.' },
      { type: 'minlength', message: 'Confirmation password must be at least 14 characters long.' }
    ],
  };

  // Variables
  countries: ICountry[] = [];
  drinkerTypes: Drinker[];
  loading = false;
  maxDate: string;
  minDate: string;
  settingsForm: FormGroup;
  sexTypes: Sex[];
  showPassword = false;
  showActualPassword = false;
  showConfirmPassword = false;
  submitted = false;

  // Getters
  get settingsFormFields() {
    return this.settingsForm.controls;
  }

  constructor(public authenticationService: AuthenticationService, private countryService: CountryService, private dialogRef: MatDialogRef<SettingsDialogComponent>,
    private jwtHelper: JwtHelperService, private snackBar: MatSnackBar, private userService: UserService) {
    this.countryService.getCountries().subscribe((countryData: ICountry[]) => {
      this.countries = countryData;
      this.countries.sort((n1, n2) => {
        if (n1.name > n2.name) {
          return 1;
        }

        if (n1.name < n2.name) {
          return -1;
        }

        return 0;
      });
    });

    this.drinkerTypes = [
      { value: DrinkerType.SocialDrinker, viewValue: 'Social drinker' },
      { value: DrinkerType.ConformityDrinker, viewValue: 'Conformity drinker' },
      { value: DrinkerType.EnhancementDrinker, viewValue: 'Enhancement drinker' },
      { value: DrinkerType.CopingDrinker, viewValue: 'Coping drinker' },
      { value: DrinkerType.None, viewValue: 'None' }
    ];

    this.sexTypes = [
      { value: SexType.Male, viewValue: 'Male' },
      { value: SexType.Female, viewValue: 'Female' },
      { value: SexType.Intersex, viewValue: 'Intersex' },
      { value: SexType.Other, viewValue: 'Other' }
    ];

    const currentDate = new Date();
    this.minDate = formatDate(new Date(currentDate.getFullYear() - 150, currentDate.getMonth(), currentDate.getDay()), 'yyyy-MM-dd', 'en-US');
    this.maxDate = formatDate(new Date(currentDate.getFullYear() - 18, currentDate.getMonth(), currentDate.getDay()), 'yyyy-MM-dd', 'en-US');

    const birthDate = this.authenticationService.currentUserValue.birthday;
    this.settingsForm = new FormGroup(
      {
        firstName: new FormControl(this.authenticationService.currentUserValue.firstName, [Validators.required]),
        lastName: new FormControl(this.authenticationService.currentUserValue.lastName, [Validators.required]),
        emailAddress: new FormControl(this.authenticationService.currentUserValue.emailAddress, [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.minLength(14)]),
        confirmPassword: new FormControl('', [Validators.minLength(14)]),
        birthday: new FormControl(formatDate(birthDate, 'yyyy-MM-dd', 'en-US'), [Validators.required]),
        country: new FormControl(this.authenticationService.currentUserValue.countryAlpha2, [Validators.required]),
        sex: new FormControl(this.authenticationService.currentUserValue.sex, [Validators.required]),
        drinkerType: new FormControl(this.authenticationService.currentUserValue.drinkerType, [Validators.required]),
        actualPassword: new FormControl('', [Validators.required, Validators.minLength(14)])
      },
      [CustomValidatorsModule.noWhitespace('password'), CustomValidatorsModule.mustMatch('password', 'confirmPassword')]
    );
  }

  submitSettings(): void {
    this.loading = true;
    this.userService.updateUser({
      firstName: this.settingsFormFields.firstName.value,
      lastName: this.settingsFormFields.lastName.value,
      emailAddress: this.settingsFormFields.emailAddress.value,
      password: this.settingsFormFields.password.value,
      countryAlpha2: this.settingsFormFields.country.value,
      birthday: this.settingsFormFields.birthday.value,
      sex: this.settingsFormFields.sex.value,
      drinkerType: this.settingsFormFields.drinkerType.value,
      actualPassword: this.settingsFormFields.actualPassword.value
    }).subscribe(() => {
      this.loading = false;
      this.dialogRef.close();
      this.snackBar.open('Settings updated successfuly.');
    }, error => {
      this.loading = false;
      this.snackBar.open(`${error.status} - ${error.statusText}\n${error.error}`);
    });
  }

  deleteAccount(): void {
    this.loading = true;
    this.userService.deleteOwnUser()
      .subscribe(() => {
        this.loading = false;
        this.dialogRef.close();
        this.snackBar.open('Account deleted successfuly.');
      }, error => {
        this.loading = false;
        this.snackBar.open(`${error.status} - ${error.statusText}\n${error.error}`);
      });
  }
}
