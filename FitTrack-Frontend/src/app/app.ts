import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastMessage } from './shared-components/toast-message/toast-message';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ToastMessage],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {}
