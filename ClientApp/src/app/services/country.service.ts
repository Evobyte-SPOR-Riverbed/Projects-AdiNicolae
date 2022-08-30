import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';

interface IDictionary {
  [index: string]: string
}

export interface ICountry {
  name: string;
  alpha2Code: string;
  flagUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Accept': 'application/json',
      'Upgrade-Insecure-Requests': '1'
    })
  }
  private countryApiUrl = 'https://restcountries.com/v3.1/';

  constructor(private httpClient: HttpClient) { }

  getCountries(): Observable<ICountry[]> {
    return this.httpClient.get<[]>(this.countryApiUrl + 'all/', this.httpOptions)
      .pipe(map(response => response.map((countryData): ICountry => ({
        name: countryData['name']['common'],
        alpha2Code: countryData['cca2'],
        flagUrl: countryData['flags']['png']
      }))), catchError(this.errorHandler));
  }

  errorHandler(error: {
    error: {
      message: string;
    }; status: any; message: any;
  }) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(errorMessage);
  }
}
