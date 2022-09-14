import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { DrinkerType, SexType } from '../helpers/enums.module';
import { MiscModule } from '../helpers/misc.module';

export interface ILoginResponse {
  accessToken: string;
}

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  emailAddress: string;
  countryAlpha2: string;
  birthday: Date;
  sex: SexType;
  drinkerType: DrinkerType;
}

export interface IUserLogin {
  emailAddress: string;
  password: string;
  rememberBrowser: boolean;
}

export interface IUserRegister {
  firstName: string;
  lastName: string;
  emailAddress: string;
  password: string;
  countryAlpha2: string;
  birthday: Date;
  sex: SexType;
  drinkerType: DrinkerType;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    })
  }
  private currentUserSubject: BehaviorSubject<IUser> | undefined;
  public currentUser: Observable<IUser> | undefined;

  constructor(@Inject('BASE_URL') private baseUrl: string, private httpClient: HttpClient, private jwtHelper: JwtHelperService, private snackBar: MatSnackBar) {
    if (this.jwtHelper.isTokenExpired(this.accessToken)) {
      this.logout();
      return;
    }

    const decodedAccessToken = this.jwtHelper.decodeToken(this.accessToken);
    this.currentUserSubject = new BehaviorSubject<IUser>({
      id: decodedAccessToken['Id'],
      firstName: decodedAccessToken['FirstName'],
      lastName: decodedAccessToken['LastName'],
      emailAddress: decodedAccessToken['EmailAddress'],
      countryAlpha2: decodedAccessToken['CountryAlpha2'],
      birthday: new Date(Date.parse(decodedAccessToken['Birthday'])),
      sex: MiscModule.fromSexString(decodedAccessToken['Sex']),
      drinkerType: MiscModule.fromDrinkerTypeString(decodedAccessToken['DrinkerType'])
    });
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public authenticate(userLogin: IUserLogin): Observable<ILoginResponse> {
    return this.httpClient.post<ILoginResponse>(`${this.baseUrl}api/users/authenticate`, JSON.stringify(userLogin), this.httpOptions)
      .pipe(map(loginResponse => {
        const decodedAccessToken = this.jwtHelper.decodeToken(loginResponse.accessToken);
        localStorage.setItem('accessToken', loginResponse.accessToken);
        this.currentUserSubject = new BehaviorSubject<IUser>({
          id: decodedAccessToken['Id'],
          firstName: decodedAccessToken['FirstName'],
          lastName: decodedAccessToken['LastName'],
          emailAddress: decodedAccessToken['EmailAddress'],
          countryAlpha2: decodedAccessToken['CountryAlpha2'],
          birthday: new Date(Date.parse(decodedAccessToken['Birthday'])),
          sex: MiscModule.fromSexString(decodedAccessToken['Sex']),
          drinkerType: MiscModule.fromDrinkerTypeString(decodedAccessToken['DrinkerType'])
        });
        this.currentUser = this.currentUserSubject.asObservable();
        return loginResponse;
      }));
  }

  public register(userRegister: IUserRegister) {
    return this.httpClient.post(`${this.baseUrl}api/users/register/`, JSON.stringify(userRegister), this.httpOptions);
  }

  public logout() {
    localStorage.removeItem('accessToken');
    this.currentUserSubject = undefined;
    this.currentUser = undefined;
  }

  public get accessToken(): string | undefined {
    return localStorage.getItem('accessToken') ?? undefined;
  }

  public get currentUserValue(): IUser {
    return this.currentUserSubject!.value;
  }

  public get isAuthenticated(): boolean {
    if (this.accessToken && this.jwtHelper.isTokenExpired(this.accessToken)) {
      this.snackBar.open('Authentication has expired, you have been logged out.');
      this.logout();
    }

    return this.currentUser && this.accessToken ? true : false;
  }
}
