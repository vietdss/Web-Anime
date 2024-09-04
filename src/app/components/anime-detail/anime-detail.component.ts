import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/services/app.service';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-anime-detail',
  templateUrl: './anime-detail.component.html',
  styleUrls: ['./anime-detail.component.css']
})
export class AnimeDetailComponent implements OnInit {
  id: any;
  animeDetail: any;
  categories: any[] = [];
  animesCategories: any[] = [];
  reviews: any[] = [];
  categoryList: string = '';
  commentForm = new FormGroup({
    Review_text: new FormControl('', [Validators.required]),
  });

  constructor(private route: ActivatedRoute, private app: AppService) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.id = +params['id'];
      this.fetchData();
    });
  }

  fetchData(): void {
    this.app.animesDetail(this.id).subscribe((data: any) => {
      this.animeDetail = data;
    });
    this.app.getAnimeCategories(this.id).subscribe((data: any) => {
      this.categoryList = data.map((c: { CategoryName: any; }) => c.CategoryName).join(', ');
    });
    this.app.categories().subscribe((data: any) => {
      this.categories = data;
    });

    this.app.animescategories().subscribe((data: any) => {
      this.animesCategories = data;
    });

    this.app.getReviewsByAnimeId(this.id).subscribe((data: any) => {
      this.reviews = data;
      this.fetchReviewUsers();
    });
  }

  

  fetchReviewUsers() {
    this.reviews.forEach(review => {
      this.app.getUser(review.UserId).subscribe((user: any) => {
        review.user = user;
        review.relativeTime = this.timeDifference(new Date(), new Date(review.Time_posted));
      });
    });
  }

  timeDifference(current: Date, previous: Date): string {
    const msPerMinute = 60 * 1000;
    const msPerHour = msPerMinute * 60;
    const msPerDay = msPerHour * 24;
    const msPerMonth = msPerDay * 30;
    const msPerYear = msPerDay * 365;

    const elapsed = current.getTime() - previous.getTime();

    if (elapsed < msPerMinute) {
      return Math.round(elapsed / 1000) + ' seconds ago';
    } else if (elapsed < msPerHour) {
      return Math.round(elapsed / msPerMinute) + ' minutes ago';
    } else if (elapsed < msPerDay) {
      return Math.round(elapsed / msPerHour) + ' hours ago';
    } else if (elapsed < msPerMonth) {
      return Math.round(elapsed / msPerDay) + ' days ago';
    } else if (elapsed < msPerYear) {
      return Math.round(elapsed / msPerMonth) + ' months ago';
    } else {
      return Math.round(elapsed / msPerYear) + ' years ago';
    }
  }

  getFirstCategoryName(): string {
    return this.animeDetail && this.animeDetail.categories && this.animeDetail.categories.length > 0
      ? this.animesCategories[0].categoryName
      : 'Unknown';
  }

  onSubmit() {
    if (this.commentForm.valid) {
      const user = JSON.parse(sessionStorage.getItem('user') || '{}');
      if (!user.UserID) {
        console.error('User ID not found in session storage');
        return;
      }
      const review = {
        AnimeId: this.id,
        UserId: user.UserID,
        Time_posted: new Date().toISOString(),
        Review_text: this.commentForm.get('Review_text')?.value
      };

      this.app.comment(review).subscribe(response => {
        this.fetchData(); // Refresh data after submission
      }, error => {
        console.error('Error submitting review', error);
      });
    } else {
      console.error('Form is not valid');
    }
  }
  onWatchNowClick() {
    // Increment view count and refresh data
    this.app.incrementViewCount(this.id).subscribe(response => {
      this.fetchData(); 
    }, error => {
      console.error('Error incrementing view count', error);
    });
  }
}
