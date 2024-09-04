import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  animes: any[] = [];
  categories: any[] = [];
  animesCategories: any[] = [];
  animesTop: any[] = [];
  animeViewsDay: any[] = [];
  animeViewsMonth: any[] = [];
  animeViewsYear: any[] = [];
  animeViewsAll: any[] = [];
  filteredAnimes: any[] = [];
  activeFilter: string = 'all';

  constructor(private app: AppService) { }

  ngOnInit(): void {
    this.app.animes().subscribe((data: any) => {
      this.animes = data;
      console.log(data);
    });

    this.app.categories().subscribe((data: any) => {
      this.categories = data;
    });

    this.app.animescategories().subscribe((data: any) => {
      this.animesCategories = data;
    });

    this.app.getAnimeViewsDay().subscribe((data: any) => {
      this.animeViewsDay = data;
      if (this.activeFilter === 'day') {
        this.filteredAnimes = this.animeViewsDay;
      }
      console.log(data);
    });

    this.app.getAnimeViewsMonth().subscribe((data: any) => {
      this.animeViewsMonth = data;
      if (this.activeFilter === 'month') {
        this.filteredAnimes = this.animeViewsMonth;
      }
      console.log(data);
    });

    this.app.getAnimeViewsYear().subscribe((data: any) => {
      this.animeViewsYear = data;
      if (this.activeFilter === 'year') {
        this.filteredAnimes = this.animeViewsYear;
      }
      console.log(data);
    });
    this.app.getAnimeViewsAll().subscribe((data: any) => {
      this.animeViewsAll = data;
      if (this.activeFilter === 'all') {
        this.filteredAnimes = this.animeViewsAll;
      }
      console.log(data);
    });
    // Initialize with day views
    this.filteredAnimes = this.animeViewsAll;
  }

  filterViews(filter: string): void {
    this.activeFilter = filter;
    if (filter === 'day') {
      this.filteredAnimes = this.animeViewsDay;
    } else if (filter === 'month') {
      this.filteredAnimes = this.animeViewsMonth;
    } else if (filter === 'all') {
      this.filteredAnimes = this.animeViewsAll;
    } else if (filter === 'year') {
      this.filteredAnimes = this.animeViewsYear;
    }
  }
}
