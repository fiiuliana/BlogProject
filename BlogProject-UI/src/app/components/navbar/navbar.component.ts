import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  isCollapsed = true;

  constructor(
    public accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  // log out the user
  logout() {
    this.accountService.logout();
    //route the user to homepage
    this.router.navigate(['/']);
  }

}
