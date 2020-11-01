import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit{
  public progress: number;
  public message: string;
  public _baseURL: string;
  public _http: HttpClient;
  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }
  ngOnInit() {
  }
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    

    this.http.post<any>(this.baseUrl + 'contract/UploadFiles', formData).subscribe(event => {
      (r) => { console.log('got r', r) }
    }, error => console.error(error));

  }
}
