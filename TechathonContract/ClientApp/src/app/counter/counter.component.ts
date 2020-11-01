import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Template } from './Template';
import { User } from './User';
import { UserTemplateMapping } from './UserTemplateMapping';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent implements OnInit{
  public currentCount = 0;
  public selected_template;
  public selected_user;
  public read_access = false;
  public write_access = false;

  public templateList:Template[];
  public userList:User[];
  public user_matrix : UserTemplateMapping[];

  private base_url = "https://localhost:5001";
  private user_api = "contract/GetAllUsers";
  private template_api = "contract/GetAllTemplate";
  private selected_user_admin;
  private _post_user_template_mapping = "contract/SaveUserTemplateMapping"
  private _get_user_template_mapping = "contract/GetAllUserTemplateMapping"

  private http:HttpClient;

  public disabled : boolean = true;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.base_url = baseUrl
  }

  ngOnInit() {
    this.getUsers( 1 );
  }

  public verifyAdmin(){
    if( ! this.userList[0].IsAdmin ){
      this.disabled = true;
    }else{
      this.disabled = false;
      this.getUsers();
      this.getTemplates();
      this.get_user_template_mapping();
    }
    console.log( this.userList );
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
    this.http.get<Template[]>(this.base_url + this.template_api).subscribe(result => {
      this.templateList = result;
    }, error => console.error(error));
  }

  public updateControl(){
    console.log( "clicked" );
    console.log( this.selected_template );
    console.log( this.selected_user );
    console.log( this.read_access );
    console.log( this.write_access );
    let data:UserTemplateMapping = new UserTemplateMapping();
    let item1 = this.templateList.find(i => i.TemplateId == this.selected_template);
    data.TemplateId = this.selected_template;
    data.TemplateName = item1.TemplateName;
    let item2 = this.userList.find(i => i.UserId == this.selected_user);
    data.UserId = this.selected_user;
    data.UserName = item2.UserName;
    data.isWrite = this.write_access ? this.write_access : this.read_access;
    this.post_user_template_mapping( data );
  }

  
  public get_user_template_mapping(){
    this.http.get<UserTemplateMapping[]>(this.base_url + this._get_user_template_mapping).subscribe(result => {
      this.user_matrix = result;
      console.log( this.user_matrix );
    }, error => console.error(error));

  }


  public post_user_template_mapping( data ){
    this.http.post(this.base_url + this._post_user_template_mapping, data).subscribe();
  }
}
