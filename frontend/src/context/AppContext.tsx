import React, { createContext, useContext, useReducer } from 'react';
import type { ReactNode } from 'react';

// Define the shape of our global UI state
interface AppState {
  // UI State
  sidebarOpen: boolean;
  theme: 'light' | 'dark';
  loading: boolean;
  
  // Notifications
  notifications: Notification[];
  
  // User preferences
  preferences: {
    language: string;
    timezone: string;
    dateFormat: string;
  };
}

interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  timestamp: number;
  autoHide?: boolean;
}

// Define action types
type AppAction =
  | { type: 'TOGGLE_SIDEBAR' }
  | { type: 'SET_SIDEBAR'; payload: boolean }
  | { type: 'SET_THEME'; payload: 'light' | 'dark' }
  | { type: 'SET_LOADING'; payload: boolean }
  | { type: 'ADD_NOTIFICATION'; payload: Omit<Notification, 'id' | 'timestamp'> }
  | { type: 'REMOVE_NOTIFICATION'; payload: string }
  | { type: 'CLEAR_NOTIFICATIONS' }
  | { type: 'SET_PREFERENCES'; payload: Partial<AppState['preferences']> }
  | { type: 'RESET_STATE' };

// Initial state
const initialState: AppState = {
  sidebarOpen: false,
  theme: 'light',
  loading: false,
  notifications: [],
  preferences: {
    language: 'en',
    timezone: 'UTC',
    dateFormat: 'MM/dd/yyyy',
  },
};

// Load state from localStorage
const loadPersistedState = (): AppState => {
  try {
    const persistedState = localStorage.getItem('appState');
    if (persistedState) {
      const parsed = JSON.parse(persistedState);
      return {
        ...initialState,
        ...parsed,
        // Don't persist notifications and loading state
        notifications: [],
        loading: false,
      };
    }
  } catch (error) {
    console.warn('Failed to load persisted state:', error);
  }
  return initialState;
};

// Reducer function
const appReducer = (state: AppState, action: AppAction): AppState => {
  switch (action.type) {
    case 'TOGGLE_SIDEBAR':
      return { ...state, sidebarOpen: !state.sidebarOpen };
    
    case 'SET_SIDEBAR':
      return { ...state, sidebarOpen: action.payload };
    
    case 'SET_THEME':
      return { ...state, theme: action.payload };
    
    case 'SET_LOADING':
      return { ...state, loading: action.payload };
    
    case 'ADD_NOTIFICATION': {
      const newNotification: Notification = {
        ...action.payload,
        id: Math.random().toString(36).substr(2, 9),
        timestamp: Date.now(),
        autoHide: action.payload.autoHide ?? true,
      };
      return {
        ...state,
        notifications: [...state.notifications, newNotification],
      };
    }
    
    case 'REMOVE_NOTIFICATION':
      return {
        ...state,
        notifications: state.notifications.filter(n => n.id !== action.payload),
      };
    
    case 'CLEAR_NOTIFICATIONS':
      return { ...state, notifications: [] };
    
    case 'SET_PREFERENCES':
      return {
        ...state,
        preferences: { ...state.preferences, ...action.payload },
      };
    
    case 'RESET_STATE':
      return initialState;
    
    default:
      return state;
  }
};

// Context type
interface AppContextType {
  state: AppState;
  dispatch: React.Dispatch<AppAction>;
  // Helper functions
  toggleSidebar: () => void;
  setSidebarOpen: (open: boolean) => void;
  setTheme: (theme: 'light' | 'dark') => void;
  setLoading: (loading: boolean) => void;
  showNotification: (notification: Omit<Notification, 'id' | 'timestamp'>) => void;
  hideNotification: (id: string) => void;
  clearNotifications: () => void;
  updatePreferences: (preferences: Partial<AppState['preferences']>) => void;
}

// Create context
const AppContext = createContext<AppContextType | undefined>(undefined);

// Provider component
interface AppProviderProps {
  children: ReactNode;
}

export const AppProvider: React.FC<AppProviderProps> = ({ children }) => {
  const [state, dispatch] = useReducer(appReducer, initialState, loadPersistedState);

  // Persist state to localStorage when it changes
  React.useEffect(() => {
    try {
      const stateToSave = {
        sidebarOpen: state.sidebarOpen,
        theme: state.theme,
        preferences: state.preferences,
      };
      localStorage.setItem('appState', JSON.stringify(stateToSave));
    } catch (error) {
      console.warn('Failed to persist state:', error);
    }
  }, [state.sidebarOpen, state.theme, state.preferences]);

  // Auto-hide notifications
  React.useEffect(() => {
    const timeouts: ReturnType<typeof setTimeout>[] = [];

    state.notifications.forEach(notification => {
      if (notification.autoHide) {
        const timeout = setTimeout(() => {
          dispatch({ type: 'REMOVE_NOTIFICATION', payload: notification.id });
        }, 5000); // 5 seconds
        timeouts.push(timeout);
      }
    });

    return () => {
      timeouts.forEach(timeout => clearTimeout(timeout));
    };
  }, [state.notifications]);

  // Helper functions
  const toggleSidebar = () => dispatch({ type: 'TOGGLE_SIDEBAR' });
  const setSidebarOpen = (open: boolean) => dispatch({ type: 'SET_SIDEBAR', payload: open });
  const setTheme = (theme: 'light' | 'dark') => dispatch({ type: 'SET_THEME', payload: theme });
  const setLoading = (loading: boolean) => dispatch({ type: 'SET_LOADING', payload: loading });
  const showNotification = (notification: Omit<Notification, 'id' | 'timestamp'>) => 
    dispatch({ type: 'ADD_NOTIFICATION', payload: notification });
  const hideNotification = (id: string) => dispatch({ type: 'REMOVE_NOTIFICATION', payload: id });
  const clearNotifications = () => dispatch({ type: 'CLEAR_NOTIFICATIONS' });
  const updatePreferences = (preferences: Partial<AppState['preferences']>) => 
    dispatch({ type: 'SET_PREFERENCES', payload: preferences });

  const contextValue: AppContextType = {
    state,
    dispatch,
    toggleSidebar,
    setSidebarOpen,
    setTheme,
    setLoading,
    showNotification,
    hideNotification,
    clearNotifications,
    updatePreferences,
  };

  return (
    <AppContext.Provider value={contextValue}>
      {children}
    </AppContext.Provider>
  );
};

// Custom hook to use the context
// eslint-disable-next-line react-refresh/only-export-components
export const useAppContext = (): AppContextType => {
  const context = useContext(AppContext);
  if (context === undefined) {
    throw new Error('useAppContext must be used within an AppProvider');
  }
  return context;
};

export default AppContext;