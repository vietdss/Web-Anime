import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-anime-search',
  templateUrl: './anime-search.component.html',
  styleUrls: ['./anime-search.component.css']
})
export class AnimeSearchComponent {
  searchQuery: any;
  animes: any;

  constructor(private route: ActivatedRoute, private app: AppService) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.searchQuery = params['querySearch'];
      this.loadAnimes();
      
    });
  }

  loadAnimes() {
    this.app.getAnimeSearch(this.searchQuery).subscribe(
      data => {
        this.animes = data;
        
      },
      error => {
        console.error('Error fetching animes', error);
        
      }
    );
  }
}
