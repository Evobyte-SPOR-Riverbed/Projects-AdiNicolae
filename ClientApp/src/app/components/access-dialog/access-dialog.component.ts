import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTabGroup } from '@angular/material/tabs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomValidatorsModule } from '../../helpers/custom-validators.module';
import { DrinkerType, SexType } from '../../helpers/enums.module';
import { AuthenticationService, LoginResponse, UserLogin } from '../../services/authentication.service';
import { CountryService, ICountry } from '../../services/country.service';

enum TabType {
  Register,
  Login
}

interface Drinker {
  value: number | DrinkerType;
  viewValue: string;
}

interface Sex {
  value: number | SexType;
  viewValue: string;
}

@Component({
  selector: 'app-access-dialog',
  templateUrl: './access-dialog.component.html',
  styleUrls: ['./access-dialog.component.css']
})
export class AccessDialogComponent {
  // Components
  @ViewChild('mainTabGroup')
  mainTabGroup!: MatTabGroup;

  // Pseudo-enums
  TabType = TabType;

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
    ]
  };

  // Variables
  countries: ICountry[] = [];
  drinkerTypes: Drinker[];
  loginForm: FormGroup;
  loading = false;
  maxDate: string;
  minDate: string;
  registerForm: FormGroup;
  sexTypes: Sex[];
  showLoginPassword = false;
  showRegisterPassword = false;
  showConfirmPassword = false;
  submitted = false;

  // Getters
  get loginFormFields() {
    return this.loginForm.controls;
  }
  get registerFormFields() {
    return this.registerForm.controls;
  }

  constructor(private authenticationService: AuthenticationService, private countryService: CountryService, private dialogRef: MatDialogRef<AccessDialogComponent>, private snackBar: MatSnackBar) {
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
    this.minDate = new Date(currentDate.getFullYear() - 150, currentDate.getMonth(), currentDate.getDay()).toISOString().slice(0, 10);
    this.maxDate = new Date(currentDate.getFullYear() - 18, currentDate.getMonth(), currentDate.getDay()).toISOString().slice(0, 10);

    this.loginForm = new FormGroup(
      {
        emailAddress: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.required]),
        rememberBrowser: new FormControl(false)
      }
    );

    this.registerForm = new FormGroup(
      {
        firstName: new FormControl('', [Validators.required]),
        lastName: new FormControl('', [Validators.required]),
        emailAddress: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.required, Validators.minLength(14), CustomValidatorsModule.noWhitespace]),
        confirmPassword: new FormControl('', [Validators.required, Validators.minLength(14)]),
        birthday: new FormControl(this.maxDate, [Validators.required]),
        country: new FormControl('', [Validators.required]),
        sex: new FormControl('', [Validators.required]),
        drinkerType: new FormControl('', [Validators.required])
      },
      CustomValidatorsModule.mustMatch('password', 'confirmPassword')
    );
  }

  submitAccess() {
    if (this.mainTabGroup.selectedIndex == TabType.Login) {
      if (this.loginForm.invalid) {
        return;
      }

      this.loading = true;
      this.authenticationService.authenticate({
        emailAddress: this.loginFormFields.emailAddress.value,
        password: this.loginFormFields.password.value,
        rememberBrowser: this.loginFormFields.rememberBrowser.value
      }).subscribe(loginResponse => {
        // localStorage.setItem("accessToken", loginResponse.accessToken);
        this.dialogRef.close();
        this.snackBar.open('Authentication was successful.');
      }, error => {
        this.loading = false;
        this.snackBar.open(`${error.statusText}\n${error.message}`);
        console.log(error);
      });
    }

    if (this.mainTabGroup.selectedIndex == TabType.Register) {
      if (this.registerForm.invalid) {
        return;
      }

      this.loading = true;
      this.authenticationService.register({
        firstName: this.registerFormFields.firstName.value,
        lastName: this.registerFormFields.lastName.value,
        emailAddress: this.registerFormFields.emailAddress.value,
        password: this.registerFormFields.password.value,
        countryAlpha2: this.registerFormFields.country.value,
        birthday: this.registerFormFields.birthday.value,
        sex: this.registerFormFields.sex.value,
        drinkerType: this.registerFormFields.drinkerType.value,
      }).subscribe(() => {
        this.loading = false;
        this.mainTabGroup.selectedIndex = TabType.Login;
        this.loginFormFields.emailAddress.setValue(this.registerFormFields.emailAddress.value);
        this.snackBar.open('Registration was successful.');
      }, error => {
        this.loading = false;
        this.snackBar.open(`${error.statusText}\n${error.message}`);
        console.log(error);
      });
    }
  }
}
