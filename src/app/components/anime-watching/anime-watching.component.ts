import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-anime-watching',
  templateUrl: './anime-watching.component.html',
  styleUrls: ['./anime-watching.component.css']
})
export class AnimeWatchingComponent {
  id: any;
  imgVideo: string='anime-watch.jpg';
  animeDetail: any;
  categories: any[] = [];
  animesCategories: any[] = [];
  videos: any[] = [];
  categoryList: string = '';
  commentForm = new FormGroup({
    Review_text: new FormControl('', [Validators.required]),
  });

  constructor(private route: ActivatedRoute, private app: AppService) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.id = +params['id'];
      console.log(this.id);
      this.fetchData();
      this.fetchVideos();
    });
  }

  fetchData(): void {
    this.app.animesDetail(this.id).subscribe((data: any) => {
      this.animeDetail = data;
      this.mapAnimesWithCategories();
      console.log(data.Ep);
    });

    this.app.categories().subscribe((data: any) => {
      this.categories = data;
      this.mapAnimesWithCategories();
    });

    this.app.animescategories().subscribe((data: any) => {
      this.animesCategories = data;
      this.mapAnimesWithCategories();
    });
  }

  fetchVideos() {
    this.app.getVideosByAnimeId(this.id).subscribe((data: any) => {
      this.videos = data;
      console.log(this.videos);
    });
  }

  mapAnimesWithCategories(): void {
    if (this.animeDetail && this.categories.length && this.animesCategories.length) {
      const categories = this.animesCategories
        .filter(ac => ac.AnimeId === this.animeDetail.Id)
        .map(ac => {
          const category = this.categories.find(category => category.CategoryId === ac.CategoryId);
          return category ? { categoryId: category.CategoryId, categoryName: category.CategoryName } : null;
        }).filter((category): category is { categoryId: any; categoryName: any } => category !== null);
      
      this.animeDetail = {
        ...this.animeDetail,
        categories: categories
      };

      this.categoryList = categories.map(c => c.categoryName).join(', ');
      console.log(this.categoryList);
    }
  }

  changeVideo(videoPath: string): void {
    const videoPlayer = <HTMLVideoElement>document.getElementById('player');
    videoPlayer.src = `assets/videos/${videoPath}`;
    videoPlayer.load();
  }
}
