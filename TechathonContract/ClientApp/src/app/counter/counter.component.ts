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
  public isRevoke = false;

  public access;

  public templateList:Template[];
  public userList:User[];
  public user_matrix : UserTemplateMapping[];

  private base_url = "https://localhost:5001";
  private user_api = "contract/GetAllUsers";
  private template_api = "contract/GetAllTemplate";
  private selected_user_admin;
  private _post_user_template_mapping = "contract/SaveUserTemplateMapping";
  private _get_user_template_mapping = "contract/GetAllUserTemplateMapping";

  private revokeUserAccess = "contract/DeleteTemplateUserMapping";

  private user_matrix_local;

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

  public data:UserTemplateMapping;
  public updateControl(){
    console.log( "clicked" );
    console.log( this.selected_template );
    console.log( this.selected_user );
    console.log( this.read_access );
    console.log(this.write_access);
    this.data = new UserTemplateMapping();
    // let item1 = this.templateList.find(i => i.TemplateId == this.selected_template);
    this.data.TemplateId = this.selected_template;
    // this.data.TemplateName = item1.TemplateName;
    // let item2 = this.userList.find(i => i.UserId == this.selected_user);
    this.data.UserId = this.selected_user;
    // this.data.UserName = item2.UserName;
    if( this.access == "revoke"){
      this.revoke(this.data);
    }else{
      if( this.access == "read" ){
        this.data.isWrite = false;
      }else if( this.access == "write" ){
        this.data.isWrite = true;
      }
      this.post_user_template_mapping(this.data);
    }
    
  }

  
  public get_user_template_mapping(){
    this.http.get<UserTemplateMapping[]>(this.base_url + this._get_user_template_mapping).subscribe(result => {
      this.user_matrix = result;
      console.log( this.user_matrix );
      this.prepare_matrix();
    }, error => console.error(error));

  }

  public prepare_matrix(){
    // this.user_matrix_local = {};
    // for (let i in this.user_matrix){
    //   this.user_matrix_local[ i.UserId ] = [];
    // }
    // for (let i in this.user_matrix){
    //   this.user_matrix_local[ i.UserId ].push( { id:i.TemplateId, isWrite:i.isWrite );
    // }
  }


  public post_user_template_mapping( data ){
    this.http.post(this.base_url + this._post_user_template_mapping + "?TemplateId=" + data.TemplateId + "&UserId=" + data.UserId + "&isWrite=" + data.isWrite, {}).subscribe(_ => {
      this.get_user_template_mapping();
    });
  }

  public revoke( data ){
    this.http.post(this.base_url + this.revokeUserAccess + "?Tid=" + data.TemplateId + "&Uid=" + data.UserId, {}).subscribe(_ => {
      this.get_user_template_mapping();
    });
  }

  public admin_message = "";
  public makeAdmin(skip=0){
    let endpoint = "contract/UpdateUserToAdmin";

    if( this.selected_user_admin ){
      this.admin_message = "";

      let item2 = this.userList.find(i => i.UserId == this.selected_user_admin);
      if( !item2.IsAdmin || skip==1 ){
        this.http.post(this.base_url + endpoint + "?id="+this.selected_user_admin+"&isAdmin="+!item2.IsAdmin, {
        }).subscribe(
          _=>this.getUsers()
        );
        this.admin_message = "";
        console.log("clicked")
      }else{
        this.admin_message = " Already Admin "
      }

    }

  }
}
