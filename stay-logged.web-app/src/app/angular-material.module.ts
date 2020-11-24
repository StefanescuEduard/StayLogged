import { NgModule } from '@angular/core';
import { LayoutModule } from '@angular/cdk/layout';

//
// Form Controls
//

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatNativeDateModule } from '@angular/material/core';

//
// Navigation
//

import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';

//
// Layout
//

import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatListModule } from '@angular/material/list';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTreeModule } from '@angular/material/tree';

//
// Buttons & Indicators
//

import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';

//
// Popups & Modals
//

import { MatDialogModule } from '@angular/material/dialog';

//
// Data Table
//

import { MatTableModule } from '@angular/material/table';
import { CdkTableModule } from '@angular/cdk/table';

//
// Effects
//

import { MatRippleModule } from '@angular/material/core';

@NgModule({
  imports: [
    LayoutModule,

    MatAutocompleteModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatSlideToggleModule,

    MatNativeDateModule,

    MatMenuModule,
    MatToolbarModule,

    MatCardModule,
    MatDividerModule,
    MatExpansionModule,
    MatListModule,
    MatStepperModule,
    MatTabsModule,
    MatTreeModule,

    MatButtonModule,
    MatButtonToggleModule,
    MatIconModule,

    MatDialogModule,

    MatTableModule,
    CdkTableModule,

    MatRippleModule
  ],
  exports: [
    LayoutModule,

    MatAutocompleteModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatSlideToggleModule,

    MatMenuModule,
    MatToolbarModule,

    MatCardModule,
    MatDividerModule,
    MatExpansionModule,
    MatListModule,
    MatStepperModule,
    MatTabsModule,
    MatTreeModule,

    MatButtonModule,
    MatButtonToggleModule,
    MatIconModule,

    MatDialogModule,

    MatTableModule,
    CdkTableModule,

    MatRippleModule
  ]
})
export class AngularMaterialModule {}
