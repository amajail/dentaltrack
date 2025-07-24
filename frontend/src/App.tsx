import { ThemeProvider } from '@mui/material/styles';
import { CssBaseline } from '@mui/material';
import theme from './theme/theme';
import AppLayout from './components/layout/AppLayout';
import DashboardPage from './pages/DashboardPage';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AppLayout>
        <DashboardPage />
      </AppLayout>
    </ThemeProvider>
  );
}

export default App;
