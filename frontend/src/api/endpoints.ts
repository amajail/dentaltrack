// API endpoint constants
export const API_ENDPOINTS = {
  // Patients
  PATIENTS: '/patients',
  PATIENT_BY_ID: (id: string) => `/patients/${id}`,
  
  // Treatments
  TREATMENTS: '/treatments',
  TREATMENT_BY_ID: (id: string) => `/treatments/${id}`,
  PATIENT_TREATMENTS: (patientId: string) => `/patients/${patientId}/treatments`,
  
  // Photos
  PHOTOS: '/photos',
  PHOTO_BY_ID: (id: string) => `/photos/${id}`,
  TREATMENT_PHOTOS: (treatmentId: string) => `/treatments/${treatmentId}/photos`,
  PATIENT_PHOTOS: (patientId: string) => `/patients/${patientId}/photos`,
  
  // Dashboard
  DASHBOARD_STATS: '/dashboard/stats',
  RECENT_ACTIVITY: '/dashboard/activity',
  
  // Reports
  REPORTS: '/reports',
  PATIENT_REPORT: (patientId: string) => `/reports/patient/${patientId}`,
  TREATMENT_REPORT: (treatmentId: string) => `/reports/treatment/${treatmentId}`,
} as const;

// Query keys for React Query
export const QUERY_KEYS = {
  // Patients
  PATIENTS: ['patients'],
  PATIENT: (id: string) => ['patients', id],
  
  // Treatments
  TREATMENTS: ['treatments'],
  TREATMENT: (id: string) => ['treatments', id],
  PATIENT_TREATMENTS: (patientId: string) => ['patients', patientId, 'treatments'],
  
  // Photos
  PHOTOS: ['photos'],
  PHOTO: (id: string) => ['photos', id],
  TREATMENT_PHOTOS: (treatmentId: string) => ['treatments', treatmentId, 'photos'],
  PATIENT_PHOTOS: (patientId: string) => ['patients', patientId, 'photos'],
  
  // Dashboard
  DASHBOARD_STATS: ['dashboard', 'stats'],
  RECENT_ACTIVITY: ['dashboard', 'activity'],
  
  // Reports
  REPORTS: ['reports'],
  PATIENT_REPORT: (patientId: string) => ['reports', 'patient', patientId],
  TREATMENT_REPORT: (treatmentId: string) => ['reports', 'treatment', treatmentId],
} as const;