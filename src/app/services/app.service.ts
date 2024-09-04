import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private baseUrl = 'https://localhost:44312/api';

  constructor(private http: HttpClient) {}

  categories(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Categories`);
  }

  animes(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/AnimeDetails`);
  }

  animescategories(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/AnimeCategories`);
  }

  animesDetail(id: any): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/AnimeDetails/${id}`);
  }

  reviews(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Reviews`);
  }
  blogs(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Blogs`);
  }
  signUp(user: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Users`, user);
  }
  comment(review: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Reviews`, review);
  }
  login(email: any, password: any): Observable<any> {
    const loginData = {
      EmailAddress: email,
      Password: password
    };
    return this.http.post<any>(`${this.baseUrl}/Users/Login`, loginData);
  }
  isLogin():any{
    let jsonData = sessionStorage.getItem('user');
    if(jsonData){
      return JSON.parse(jsonData);
    }
    return false;
  }
  getUser(userId: any) {
    return this.http.get(`${this.baseUrl}/Users/${userId}`);
  }
  getVideosByAnimeId(animeId: any): any {
    return this.http.get(`${this.baseUrl}/Videos/ByAnime/${animeId}`);
  }
  getReviewsByAnimeId(animeId: any): any {
    return this.http.get(`${this.baseUrl}/Reviews/ByAnime/${animeId}`);
  }
  getBlogsLatest(count: number): any {
    return this.http.get(`${this.baseUrl}/Blogs/latest?count=${count}`);
  }
  getBlogById(blogId: any): any {
    return this.http.get(`${this.baseUrl}/Blogs/${blogId}`);
  }
  getAnimeByCategory(categoryName: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/Category/${categoryName}`);
  }
  getAnimeViewsDay(): Observable<any[]> {
    const currentDate = new Date().toISOString().split('T')[0]; // Getting the current date in 'YYYY-MM-DD' format
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/SortedByViews/Day/${currentDate}`);
  }
  getAnimeViewsMonth(): Observable<any[]> {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth() + 1; // Getting the current month (months are 0-based)
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/SortedByViews/Month/${year}/${month}`);
  }
  getAnimeViewsAll(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/SortedByViews/All`);
  }
  getAnimeViewsYear(): Observable<any[]> {
    const currentDate = new Date();
    const year = currentDate.getFullYear(); // Getting the current year
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/SortedByViews/Year/${year}`);
  }
  incrementViewCount(animeId: number): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/ViewCounts/IncrementViews/${animeId}`, {});
  }
  getAnimeCategories(animeId: number): Observable<any> {
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/AnimeCategories/${animeId}`);
  }
  getViewAnime(animeId: number): Observable<any> {
    return this.http.get<any[]>(`${this.baseUrl}/ViewCounts/Anime/${animeId}`);
  }
  getAnimeSearch(querySearch: number): Observable<any> {
    return this.http.get<any[]>(`${this.baseUrl}/AnimeDetails/Search/${querySearch}`);
  }

}
