import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';
import { environment } from 'src/environments/environment';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(
    private accountService: AccountService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    const currentUser = this.accountService.currentUserValue;

    //if there is an user and a token the user is logged in
    const isLoggedIn = currentUser && currentUser.token;
    const isApiUrl = request.url.startsWith(environment.webApi);
    
    // the bearer is added globally
    if (this.accountService.isLoggedIn() && isApiUrl){
        request = request.clone({
          setHeaders:{
            Authorization: `bearer ${currentUser.token}`
          }})
    }

    return next.handle(request);
  }
}
