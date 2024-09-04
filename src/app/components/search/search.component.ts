import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  searchQuery: string='';

  constructor(private router: Router) {}

  onSubmit() {
    this.router.navigate(['/anime-search'], { queryParams: { querySearch: this.searchQuery } });
    console.log(this.searchQuery);
    
  }
}
