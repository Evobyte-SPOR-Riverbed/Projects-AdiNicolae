import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, Observable, throwError } from 'rxjs';
import { DrinkerType, SexType } from '../helpers/enums.module';
import { AuthenticationService } from './authentication.service';

export interface ISettingsUser {
  firstName: string;
  lastName: string;
  emailAddress: string;
  password: string;
  countryAlpha2: string;
  birthday: Date;
  sex: SexType;
  drinkerType: DrinkerType;
  actualPassword: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    })
  };

  constructor(private authenticationService: AuthenticationService, @Inject('BASE_URL') private baseUrl: string, private httpClient: HttpClient) {
    this.authenticationService.isAuthenticated;
  }

  updateUser(settingsUser: ISettingsUser) {
    return this.httpClient.put(`${this.baseUrl}api/users/${this.authenticationService.currentUserValue.id}`, settingsUser, this.httpOptions);
  }

  deleteOwnUser() {
    return this.httpClient.delete(`${this.baseUrl}api/users/${this.authenticationService.currentUserValue.id}`, this.httpOptions);
  }
}
