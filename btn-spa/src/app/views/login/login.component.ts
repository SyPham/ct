import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_core/_service/auth.service';
import { AlertifyService } from '../../_core/_service/alertify.service';
import { Router, ActivatedRoute, RouterStateSnapshot } from '@angular/router';
const ADMIN = 1;
const SUPERVISOR = 2;
const STAFF = 3;
const WORKER = 4;
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user: any = {};
  uri: any;
  level: number;
 
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private alertifyService: AlertifyService
  ) {
    this.route.queryParams.subscribe(params => {
      this.uri = params.uri;
    });
  }
  role: number;
  ngOnInit(): void {
  }
  login(): void {
    this.authService.login(this.user).subscribe(
      next => {
        this.role = JSON.parse(localStorage.getItem('user')).User.Role;
        this.alertifyService.success('Login Success!!');
        this.authService.getBuildingByUserID(JSON.parse(localStorage.getItem('user')).User.ID).subscribe((res: any) => {
          res = res || {};
          localStorage.setItem('level', JSON.stringify(res));
          this.level = res.level;
          if (this.level === WORKER) {
            const currentLang = localStorage.getItem('lang');
            if (currentLang) {
              localStorage.setItem('lang', currentLang);
            } else {
              localStorage.setItem('lang', 'vi');
            }
          } else {
            const currentLang = localStorage.getItem('lang');
            if (currentLang) {
              localStorage.setItem('lang', currentLang);
            } else {
              localStorage.setItem('lang', 'en');
            }
          }
          this.checkRole();
        });
      },
      error => {
        this.alertifyService.error('Login failed!!');
      },
      () => {
      }
    );
  }
  checkRole() {
    const uri = decodeURI(this.uri);
    if (uri !== 'undefined') {
      this.router.navigate(['/ec/setting/line']);
    } else {
      this.router.navigate(['/ec/setting/line']);
    }
  }
}
