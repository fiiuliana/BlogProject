import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';
import { catchError } from 'rxjs/operators';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    //dependencies
    private toastr: ToastrService,
  //  private router: Router,
    private accountService: AccountService

  ) {}

  // we have to pipe into the observable
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error){
            switch(error.status){
              case 400:
                this.handle400Error(error);
              break;
              case 401:
                this.handle401Error(error);
              break;
              case 500:
                this.handle500Error(error);
              break;
              default:
                //a method that handles unexpected error
                this.handleUnexpectedError(error);
                break;
            }
        }
        return throwError(error);
      })
    );
  }

  handle400Error(error: any) {
    if (!!error.error && Array.isArray(error.error)) {
      let errorMessage = '';
      //iterating inside the error array
      for (const key in error.error) {
        if (!!error.error[key]) {
          const errorElement = error.error[key];
          //show all erors
          errorMessage = (`${errorMessage}${errorElement.code} - ${errorElement.description}\n`);
        }
      }
      // show the error mesage and the status text
      this.toastr.error(errorMessage, error.statusText);
      //to see what is happening - what that error was
      console.log(error.error);
      //if there is a content and has a specific type === getting back
    } else if (!!error?.error?.errors?.Content && (typeof error.error.errors.Content) === 'object') {
      let errorObject = error.error.errors.Content;
      let errorMessage = '';
      for (const key in errorObject) {
        const errorElement = errorObject[key];
        errorMessage = (`${errorMessage}${errorElement}\n`);
      }
      this.toastr.error(errorMessage, error.statusCode);
      console.log(error.error);
    } else if (!!error.error) {
      let errorMessage = ((typeof error.error) === 'string')  // if string just show it
        ? error.error
        : 'There was a validation error.';   //else ... show the message
      this.toastr.error(errorMessage, error.statusCode);
      console.log(error.error);
    } else {
      this.toastr.error(error.statusText, error.status);
      console.log(error);
    }
  }

  handle401Error(error: any) {  // happens when the user is not authorised - token expired for example
    let errorMessage = 'Please login to your account.';
    this.accountService.logout();
    this.toastr.error(errorMessage, error.statusText);
    //route to the login page
   // this.router.navigate(['/login']);
  }

  handle500Error(error: any) {
    this.toastr.error('Unknown error. An error happened in the server.');
    console.log(error);
  }
  
  handleUnexpectedError(error: any){
    this.toastr.error('Something unexpected happened.');
    console.log(error);
  }

}
