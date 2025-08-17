import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component'; // standalone component

@NgModule({
  // ❌ Remove declarations
  // declarations: [AppComponent],

  // ✅ Move AppComponent to imports
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    AppComponent // <-- Import it here
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
