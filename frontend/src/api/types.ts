export interface ApiResponse<T = unknown> {
  data: T;
  message: string;
  success: boolean;
}

export interface ApiError {
  message: string;
  status: number;
  data?: unknown;
}

export interface PaginatedResponse<T> {
  data: T[];
  total: number;
  page: number;
  limit: number;
  totalPages: number;
}

export interface Patient {
  id: string;
  name: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  createdAt: string;
  updatedAt: string;
}

export interface Treatment {
  id: string;
  patientId: string;
  type: string;
  status: 'active' | 'completed' | 'cancelled';
  startDate: string;
  endDate?: string;
  progress: number;
  notes: string;
  createdAt: string;
  updatedAt: string;
}

export interface Photo {
  id: string;
  patientId: string;
  treatmentId: string;
  url: string;
  type: 'before' | 'during' | 'after';
  capturedAt: string;
  createdAt: string;
}