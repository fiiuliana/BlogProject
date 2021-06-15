import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApplicationUserLogin } from 'src/app/models/account/application-user-login.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  constructor(

    private accountService: AccountService,
    private router: Router,
    // in order to make reactive and template form validation
    private formBuilder: FormBuilder
  ) { 
    // if the user is logged in - redirect to the dashboard
    if (this.accountService.isLoggedIn()) {
        this.router.navigate(['/dashboard']);
    }
  }
  // hook into a lifecicle of a specific component
  ngOnInit(): void {
       this.loginForm = this.formBuilder.group({  //a username filed wich is null and has some validators
      username: [null, [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(20)
      ]],
      password: [null, [  //has initially a null password and has validators
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(50)
      ]]
    });
  }
  // is the form touched or not
  isTouched(field: string){
    return this.loginForm.get(field).touched;
  }
  // are there errors in the form - calculates if there is an error or not
  hasErrors(field: string) {
    return this.loginForm.get(field).errors;
  }

  // there is a specific error
  hasError(field: string, error: string) {
    return !!this.loginForm.get(field).hasError(error);
  }

  onSubmit() {
    //create another instance and pass in the info from the form
    let applicationUserLogin: ApplicationUserLogin = new ApplicationUserLogin(
      this.loginForm.get("username").value,
      this.loginForm.get("password").value
    );

    this.accountService.login(applicationUserLogin).subscribe(() => {
      // an observable is reurned -> can use subscribe -> navigate to dashboard
      this.router.navigate(['/dashboard']);
    });
  }

}
