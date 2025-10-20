import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Employee } from '../../models/employee';
import { EmployeeService } from '../employee.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})

export class EmployeeFormComponent implements OnInit  {

  constructor(private employeeService: EmployeeService, private router: Router, private activatedRoute: ActivatedRoute) {}
  
  employee: Employee = {
    id: 0,
    firstName: '',
    lastName: '',
    position: '',
    phoneNumber: '',
    email: '',
    salary: 0,
    birthDate: new Date()
  };

  // Helper string bound to the date input (yyyy-MM-dd). Kept in sync with employee.birthDate
  birthDateString: string = this.formatDateForInput(this.employee.birthDate);

  errorMessage: string = '';
  isEditing: boolean = false;
  formVisible: boolean = false;

  ngOnInit(): void {
    
    this.formVisible = true;
    const employeeId = this.activatedRoute.snapshot.paramMap.get('id');
    if (employeeId) {
      this.isEditing=true;
      this.employeeService.getEmployeeById(+employeeId).subscribe({
        next: (result) => {
          this.employee = result;
          // ensure the string representation is in sync after we get data from service
          this.birthDateString = this.formatDateForInput(this.employee.birthDate);
        },
        error: (error) => {
          console.error('Error fetching employee:', error);
          this.errorMessage = 'Failed to load employee data.';
          this.formVisible = false;
        }
      });
    }
  }

  // Format a Date (or date-like value) into yyyy-MM-dd for HTML date inputs
  private formatDateForInput(d: Date | string | undefined | null): string {
    if (!d) return '';
    const date = d instanceof Date ? d : new Date(d);
    if (isNaN(date.getTime())) return '';
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  // Parse yyyy-MM-dd into a Date and assign to employee.birthDate
  onBirthDateChange(value: string) {
    this.birthDateString = value;
    if (!value) {
      this.employee.birthDate = new Date(''); // invalid empty date
      return;
    }
    const parts = value.split('-');
    if (parts.length !== 3) return;
    const [y, m, d] = parts.map((p) => parseInt(p, 10));
    // month is 0-based in Date
    this.employee.birthDate = new Date(y, m - 1, d);
  }

  onSubmit(form?: NgForm) {
    // If form provided, validate before submitting
    if (form && form.invalid) {
      // mark controls as touched to show validation messages
      Object.values(form.controls).forEach((c) => c.markAsTouched());
      this.errorMessage = 'Please fix validation errors before submitting.';
      return;
    }

    if (this.isEditing) {
      this.updateEmployee();
    } else {
      this.createEmployee();
    }
  }

  createEmployee() {
    console.log('Creating employee:', this.employee);
    this.employeeService.createEmployee(this.employee).subscribe({
      next: (response) => {
        console.log('Employee created successfully', response);
        this.router.navigate(['/employees']);
      },
      error: (error) => {
        console.error('Error creating employee', error);
        this.errorMessage = `Failed to create employee. Please try again. status: ${error.status}, message: ${error.message} `;
      }
    });
  } 
  updateEmployee() {
    console.log('Updating employee:', this.employee);
    this.employeeService.updateEmployee(this.employee).subscribe({
      next: (response) => {
        console.log('Employee updated successfully', response);
        this.router.navigate(['/employees']);
      },
      error: (error) => {
        console.error('Error updating employee', error);
        this.errorMessage = `Failed to update employee. Please try again. status: ${error.status}, message: ${error.message} `;
      }
    });
  }

  cancel() {
    console.log('Employee creation/editing cancelled');
    // Here you would typically reset the form or navigate away
  }
}
