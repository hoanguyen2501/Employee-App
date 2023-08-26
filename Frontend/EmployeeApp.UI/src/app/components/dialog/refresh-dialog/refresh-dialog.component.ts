import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-refresh-dialog',
  templateUrl: './refresh-dialog.component.html',
  styleUrls: ['./refresh-dialog.component.css'],
})
export class RefreshDialogComponent {
  constructor(
    private dialogRef: MatDialogRef<RefreshDialogComponent>,
    private authService: AuthService,
    private toastr: ToastrService
  ) {
    this.dialogRef.disableClose = true;
  }

  onRefresh(): void {
    this.authService.refresh().subscribe({
      next: () => {
        this.authService.autoLogout();
        this.toastr.success('Your session was refreshed!');
      },
      error: () => {
        this.toastr.error('Cannot refresh your session, please login again!');
        this.authService.logout();
      },
    });
  }
}
