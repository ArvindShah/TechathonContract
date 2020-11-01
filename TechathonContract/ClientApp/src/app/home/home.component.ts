import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { User } from '../counter/User';
import { Template } from '../counter/Template';
import { Clause } from './Clause';

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
  public http: HttpClient;

  public user_id;

  public selected_version;

  public selected_template;
  private user_api = "contract/GetAllUsers";

  private template_api = "contract/GetAllTemplateByUserId";


  public templateList:Template[];
  public userList:User[];
  

  public base_url : string;
  constructor(private __http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
      this.http = __http;
      this.base_url = baseUrl;
  }
  ngOnInit() {
    this.getUsers( 1 );
  }

  public verifyAdmin(){
    this.user_id = this.userList[0].UserId;
    this.getTemplates();
    this.getClause();
  }




  public getUsers( verifyAdmin:number=0 ){
    let user_api_;
    if( verifyAdmin > 0 ){
      user_api_= this.user_api + "?UserId=1";
    }
    else{
      user_api_ = this.user_api;
    }
    this.http.get<User[]>(this.base_url + user_api_ ).subscribe(result => {
      this.userList = result;
      console.log( this.userList );
      if( verifyAdmin > 0 ){
        this.verifyAdmin();
      }
    }, error => console.error(error));
  }

  public getTemplates( ){
    this.http.get<Template[]>(this.base_url + this.template_api+"?UserId="+this.user_id).subscribe(result => {
      this.templateList = result;
    }, error => console.error(error));
  }

  public versionList : string[];

  public getVerions(  ){
    let endpoint = "contract/GetContentDataV"
    this.http.get<string[]>(this.base_url + endpoint+"?TemplateId="+this.selected_template).subscribe(result => {
      this.versionList = result;
    }, error => console.error(error));
  }

  public clauseList:Clause[];


  public getClause(){
    let endpoint = "contract/GetContentData?IsClause=true";
    this.http.get<Clause[]>(this.base_url + endpoint).subscribe(result => {
      this.clauseList = result;
    }, error => console.error(error));
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
