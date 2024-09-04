import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-blog-detail',
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.css']
})
export class BlogDetailComponent {
  id: any;
  Blog: any;

  constructor(private route: ActivatedRoute, private app: AppService) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.id = +params['id'];
    });
  }
  fetchData(): void {
    this.app.getBlogById(this.id).subscribe((data: any) => {
      this.Blog = data;
    });
  }
}
