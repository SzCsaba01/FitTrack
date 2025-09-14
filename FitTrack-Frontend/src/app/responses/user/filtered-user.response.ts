import { FilteredUser } from '../../models/filtered-user.model';

export interface FilteredUserResponse {
  users: FilteredUser[];
  totalNumberOfUsers: number;
  totalNumberOfPages: number;
}
