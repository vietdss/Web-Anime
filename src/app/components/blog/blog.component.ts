import { Component } from '@angular/core';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent {
  Blogs: any;

  blogCount: number = 12; 
  constructor(private app: AppService) {

  } 
  ngOnInit(): void {
    this.app.getBlogsLatest(this.blogCount).subscribe((data: any) => {
      this.Blogs = data;
    });
    console.log(this.Blogs)
 
  }

}
