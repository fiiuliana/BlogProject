import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private accountService: AccountService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    
    state: RouterStateSnapshot): boolean { // changed from default to only return a boolean
      // protect the route
        const currentUser = this.accountService.currentUserValue;
        const isLoggedIn = currentUser && currentUser.token;

        //if the current exists 
        if (isLoggedIn) {
          return true;
        }
      
        this.router.navigate(['/']);
        return false;


  }
  
}
