import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  category: any;
  animes: any;

  constructor(private route: ActivatedRoute, private app: AppService) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.category = params['CategoryName'];
      console.log(this.category)
      this.loadAnimes();
      console.log(this.animes)
    });
  }

  loadAnimes() {
    this.app.getAnimeByCategory(this.category).subscribe(
      data => {
        this.animes = data;
      },
      error => {
        console.error('Error fetching animes', error);
      }
    );
  }
}
