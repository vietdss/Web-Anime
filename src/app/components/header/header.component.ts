import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/services/app.service';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  Categories: any;
  isLogin: any;
  constructor(private app: AppService) {

  } 
  ngOnInit(): void {
    this.app.categories().subscribe((data: any) => {
      this.Categories = data;
    });
    this.isLogin=this.app.isLogin();
  }
  logout(){
    sessionStorage.removeItem('user');
    location.reload();
  }
}
