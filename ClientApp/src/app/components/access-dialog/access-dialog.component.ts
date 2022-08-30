import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomValidatorsModule } from '../../helpers/custom-validators.module';
import { CountryService, ICountry } from '../../services/country.service';

enum DrinkerType {
  SocialDrinker,
  ConformityDrinker,
  EnhancementDrinker,
  CopingDrinker,
  None
}

enum SexType {
  Male,
  Female,
  Intersex,
  Other
}

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

  constructor(private countryService: CountryService) {
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
        remember: new FormControl(false)
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

  get loginFormFields() { return this.loginForm.controls; }
  get registerFormFields() { return this.registerForm.controls; }
}
