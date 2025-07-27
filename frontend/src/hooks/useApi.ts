import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import type { UseQueryOptions, UseMutationOptions } from '@tanstack/react-query';
import type { AxiosResponse } from 'axios';
import apiClient from '../api/client';
import type { ApiResponse, ApiError } from '../api/types';

// Generic hook for GET requests
export function useApiQuery<TData = unknown, TError = ApiError>(
  key: string[],
  url: string,
  options?: Omit<UseQueryOptions<AxiosResponse<ApiResponse<TData>>, TError>, 'queryKey' | 'queryFn'>
) {
  return useQuery({
    queryKey: key,
    queryFn: () => apiClient.get<ApiResponse<TData>>(url),
    ...options,
  });
}

// Generic hook for POST requests
export function useApiMutation<TData = unknown, TVariables = unknown, TError = ApiError>(
  url: string,
  options?: UseMutationOptions<AxiosResponse<ApiResponse<TData>>, TError, TVariables>
) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: TVariables) => apiClient.post<ApiResponse<TData>>(url, data),
    onSuccess: (data, variables, context) => {
      // Invalidate and refetch related queries
      queryClient.invalidateQueries({ queryKey: [url.split('/')[1]] });
      options?.onSuccess?.(data, variables, context);
    },
    ...options,
  });
}

// Generic hook for PUT requests
export function useApiUpdateMutation<TData = unknown, TVariables = unknown, TError = ApiError>(
  url: string,
  options?: UseMutationOptions<AxiosResponse<ApiResponse<TData>>, TError, TVariables>
) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: TVariables) => apiClient.put<ApiResponse<TData>>(url, data),
    onSuccess: (data, variables, context) => {
      // Invalidate and refetch related queries
      queryClient.invalidateQueries({ queryKey: [url.split('/')[1]] });
      options?.onSuccess?.(data, variables, context);
    },
    ...options,
  });
}

// Generic hook for DELETE requests
export function useApiDeleteMutation<TData = unknown, TVariables = unknown, TError = ApiError>(
  url: string,
  options?: UseMutationOptions<AxiosResponse<ApiResponse<TData>>, TError, TVariables>
) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: TVariables) => apiClient.delete<ApiResponse<TData>>(`${url}/${id}`),
    onSuccess: (data, variables, context) => {
      // Invalidate and refetch related queries
      queryClient.invalidateQueries({ queryKey: [url.split('/')[1]] });
      options?.onSuccess?.(data, variables, context);
    },
    ...options,
  });
}

// Hook for optimistic updates
export function useOptimisticUpdate() {
  const queryClient = useQueryClient();

  const updateOptimistically = <TData>(
    queryKey: string[],
    updater: (oldData: TData | undefined) => TData
  ) => {
    queryClient.setQueryData(queryKey, updater);
  };

  const rollbackOptimisticUpdate = (queryKey: string[], previousData: unknown) => {
    queryClient.setQueryData(queryKey, previousData);
  };

  return {
    updateOptimistically,
    rollbackOptimisticUpdate,
  };
}