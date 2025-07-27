import React from 'react';
import { Snackbar, Alert, AlertTitle, Slide } from '@mui/material';
import type { SlideProps } from '@mui/material';
import { useAppContext } from '../../context/AppContext';

function SlideTransition(props: SlideProps) {
  return <Slide {...props} direction="up" />;
}

const NotificationSystem: React.FC = () => {
  const { state, hideNotification } = useAppContext();
  const { notifications } = state;

  // Show the most recent notification
  const currentNotification = notifications[notifications.length - 1];

  const handleClose = (_event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    if (currentNotification) {
      hideNotification(currentNotification.id);
    }
  };

  return (
    <Snackbar
      open={!!currentNotification}
      autoHideDuration={currentNotification?.autoHide ? 5000 : null}
      onClose={handleClose}
      TransitionComponent={SlideTransition}
      anchorOrigin={{
        vertical: 'bottom',
        horizontal: 'right',
      }}
    >
      {currentNotification && (
        <Alert
          onClose={handleClose}
          severity={currentNotification.type}
          variant="filled"
          sx={{ minWidth: 300 }}
        >
          {currentNotification.type === 'error' && (
            <AlertTitle>Error</AlertTitle>
          )}
          {currentNotification.message}
        </Alert>
      )}
    </Snackbar>
  );
};

export default NotificationSystem;