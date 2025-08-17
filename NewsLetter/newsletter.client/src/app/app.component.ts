import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor } from '@angular/common'; // 
import { FormsModule } from '@angular/forms';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  standalone: true, 
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [NgIf, NgFor, FormsModule] 
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  private http = inject(HttpClient);

  rftaList: any[] = [];
  filteredList: any[] = [];
  currentPage = 1;
  pageSize = 10;
  totalRecords = 0;
  searchTerm = '';
  loading = false;



  private apiUrl = '/weatherforecast';


  ngOnInit() {
    this.getRftaList();
  }

  getRftaList(): void {
    const params = new HttpParams()
      .set('page', this.currentPage.toString())
      .set('pageSize', this.pageSize.toString())
      .set('query', this.searchTerm.toString());




    this.loading = true;

    this.http
      .get<{ items: any[]; total: number; page: number; pageSize: number }>(this.apiUrl, { params })
      .subscribe({
        next: (res) => {
          this.rftaList = res.items;
          this.totalRecords = res.total;
          this.currentPage = res.page;
          this.pageSize = res.pageSize;
          this.filteredList = res.items;
          this.loading = false;
          console.log(this.filteredList, 'ress')
        },
        error: (err) => {
          console.error('Error fetching API:', err);
          this.loading = false;
        },
      });
  }

  get totalPages() {
    return Math.ceil(this.totalRecords / this.pageSize) || 1;
  }

  onPageChange(newPage: number): void {
    if (newPage < 1 || newPage > this.totalPages) return;
    this.currentPage = newPage;
    this.getRftaList();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.getRftaList();
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase().trim();
    if (term.length >= 3) {
      this.getRftaList();
    }
  }

 

  title = 'newsletter.client';
}
