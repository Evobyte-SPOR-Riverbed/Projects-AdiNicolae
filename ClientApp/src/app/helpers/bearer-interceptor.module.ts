import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class BearerInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, @Inject('BASE_URL') private baseUrl: string) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!request.url.includes(this.baseUrl)) {
      return next.handle(request);
    }

    if (this.authenticationService.isAuthenticated) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.authenticationService.accessToken}`
        }
      });
    }

    return next.handle(request);
  }
}
