import React from 'react';
import { Grid, type GridProps } from '@mui/material';

interface ResponsiveGridProps extends Omit<GridProps, 'container' | 'item'> {
  children: React.ReactNode;
  spacing?: number;
  type?: 'container' | 'item';
  xs?: number | boolean;
  sm?: number | boolean;
  md?: number | boolean;
  lg?: number | boolean;
  xl?: number | boolean;
}

const ResponsiveGrid: React.FC<ResponsiveGridProps> = ({
  children,
  spacing = 2,
  type = 'container',
  xs,
  sm,
  md,
  lg,
  xl,
  ...props
}) => {
  const gridProps: Record<string, unknown> = {
    ...props,
    [type]: true,
  };

  if (type === 'container') {
    gridProps.spacing = spacing;
  }

  if (type === 'item') {
    if (xs !== undefined) gridProps.xs = xs;
    if (sm !== undefined) gridProps.sm = sm;
    if (md !== undefined) gridProps.md = md;
    if (lg !== undefined) gridProps.lg = lg;
    if (xl !== undefined) gridProps.xl = xl;
  }

  return <Grid {...gridProps}>{children}</Grid>;
};

export default ResponsiveGrid;