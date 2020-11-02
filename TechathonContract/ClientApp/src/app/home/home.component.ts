import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { User } from '../counter/User';
import { Template } from '../counter/Template';
import { Clause } from './Clause';
import { ContentType } from './ContentType';
import { ContentControl } from './ContentControl';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit{
  public model = {
    editorData: '<p>GEP Contract Editor</p>',
    readOnly: false,
    Edit: false,
    first: false,
    editContent: false,
    ContentDesc:''
  };
  public progress: number;
  public message: string;
  public _baseURL: string;
  public _http: HttpClient;
  public http: HttpClient;
  public theHtmlString: any;
  public user_id;

  public selected_version;

  public selected_template;
  public selected_templateType;
  public selected_ContentControl;
  private user_api = "contract/GetAllUsers";

  private template_api = "contract/GetAllTemplateByUserId";


  public templateList:Template[];
  public templateTypeList: ContentType[];
  public userList:User[];
  public contentControl: ContentControl[];
  

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
    this.getContentType();
  }

  public contentControldesc;
  public getContentControlDesc() {
    this.contentControl.forEach((element) => {
      if (element.Tag == this.selected_ContentControl) {
        this.contentControldesc = element.Content;
      }
    });
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

  public getContentType() {
    this.http.get<ContentType[]>(this.base_url + "contract/GetContentType").subscribe(result => {
      this.templateTypeList = result;
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

  public showFile = () => {
    this.model.Edit = false;
    this.model.first = true;
    this.model.editContent = false;
    this.http.get<any>(this.baseUrl + 'contract/GetUploadedDoc?docName=Master Agreement_Template&version=1.0.0.1').subscribe(result => {
      this.theHtmlString = result.m_StringValue;
      this.model.editorData = result.m_StringValue;
    }, error => console.error(error));
  }

  public EditFile = () => {
    this.model.Edit = true;
    this.model.readOnly = false;
    this.model.editContent = false;

    this.http.get<any>(this.baseUrl + 'contract/GetUploadedDoc?docName=Master Agreement_Template&version=1.0.0.1').subscribe(result => {
      this.theHtmlString = result.m_StringValue;
      this.model.editorData = result.m_StringValue;
    }, error => console.error(error));
  }

  public EditContent = () => {
    this.model.Edit = false;
    this.model.first = false;
    this.model.editContent = true;

    this.http.get<ContentControl[]>(this.baseUrl + 'contract/GetContentControl?fileName=Clause Template11&version=1.0.0.1').subscribe(result => {
      this.contentControl = result;
    }, error => console.error(error));
  }

  public CheckOutFile = () => {
    this.model.Edit = false;
    this.model.first = false;
    this.model.editContent = true;

    this.http.get<void>(this.baseUrl + 'contract/CheckOutContentControl?fileName=Clause Template11&version=1.0.0.1&tag=' + this.selected_ContentControl + '&content=' + this.contentControldesc).subscribe(result => {
    }, error => console.error(error));
  }

}
