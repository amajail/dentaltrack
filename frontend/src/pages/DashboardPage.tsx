import React from 'react';
import {
  Typography,
  Card,
  CardContent,
  CardActions,
  Button,
  Box,
  Chip,
  Avatar,
} from '@mui/material';
import {
  TrendingUp as TrendingUpIcon,
  People as PeopleIcon,
  PhotoCamera as PhotoIcon,
  Assignment as AssignmentIcon,
} from '@mui/icons-material';
import ResponsiveGrid from '../components/common/ResponsiveGrid';

const DashboardPage: React.FC = () => {
  const statsCards = [
    {
      title: 'Active Patients',
      value: '24',
      change: '+12%',
      icon: <PeopleIcon />,
      color: 'primary',
    },
    {
      title: 'Active Treatments',
      value: '18',
      change: '+8%',
      icon: <AssignmentIcon />,
      color: 'secondary',
    },
    {
      title: 'Photos Captured',
      value: '156',
      change: '+24%',
      icon: <PhotoIcon />,
      color: 'success',
    },
    {
      title: 'Progress Reports',
      value: '42',
      change: '+15%',
      icon: <TrendingUpIcon />,
      color: 'info',
    },
  ];

  const recentPatients = [
    { name: 'Maria García', treatment: 'Teeth Whitening', progress: '75%' },
    { name: 'Juan Pérez', treatment: 'Orthodontic', progress: '45%' },
    { name: 'Ana López', treatment: 'Teeth Whitening', progress: '90%' },
  ];

  return (
    <Box>
      <Typography variant="h4" component="h1" gutterBottom>
        Dashboard
      </Typography>
      <Typography variant="body1" color="text.secondary" paragraph>
        Welcome to DentalTrack. Monitor your patients' progress and manage treatments efficiently.
      </Typography>

      <ResponsiveGrid spacing={3}>
        {statsCards.map((card, index) => (
          <ResponsiveGrid key={index} type="item" xs={12} sm={6} md={3}>
            <Card sx={{ height: '100%' }}>
              <CardContent>
                <Box display="flex" alignItems="center" justifyContent="space-between" mb={2}>
                  <Avatar sx={{ bgcolor: `${card.color}.main` }}>
                    {card.icon}
                  </Avatar>
                  <Chip
                    label={card.change}
                    color={card.color as 'primary' | 'secondary' | 'success' | 'info'}
                    size="small"
                    variant="outlined"
                  />
                </Box>
                <Typography variant="h4" component="div" gutterBottom>
                  {card.value}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {card.title}
                </Typography>
              </CardContent>
            </Card>
          </ResponsiveGrid>
        ))}
      </ResponsiveGrid>

      <Box mt={4}>
        <ResponsiveGrid spacing={3}>
          <ResponsiveGrid type="item" xs={12} md={8}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Recent Activity
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Latest updates from your dental practice
                </Typography>
                <Box mt={2}>
                  <Typography variant="body2" paragraph>
                    • New patient Maria García started teeth whitening treatment
                  </Typography>
                  <Typography variant="body2" paragraph>
                    • Progress photos uploaded for Juan Pérez orthodontic treatment
                  </Typography>
                  <Typography variant="body2" paragraph>
                    • Treatment completed for Ana López - 90% whitening improvement
                  </Typography>
                </Box>
              </CardContent>
              <CardActions>
                <Button size="small">View All Activities</Button>
              </CardActions>
            </Card>
          </ResponsiveGrid>

          <ResponsiveGrid type="item" xs={12} md={4}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Recent Patients
                </Typography>
                {recentPatients.map((patient, index) => (
                  <Box key={index} display="flex" alignItems="center" py={1}>
                    <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
                      {patient.name.charAt(0)}
                    </Avatar>
                    <Box flexGrow={1}>
                      <Typography variant="body2" fontWeight="medium">
                        {patient.name}
                      </Typography>
                      <Typography variant="caption" color="text.secondary">
                        {patient.treatment}
                      </Typography>
                    </Box>
                    <Box textAlign="right">
                      <Chip
                        label={patient.progress}
                        size="small"
                        color="primary"
                        variant="outlined"
                      />
                    </Box>
                  </Box>
                ))}
              </CardContent>
              <CardActions>
                <Button size="small" fullWidth>
                  View All Patients
                </Button>
              </CardActions>
            </Card>
          </ResponsiveGrid>
        </ResponsiveGrid>
      </Box>
    </Box>
  );
};

export default DashboardPage;