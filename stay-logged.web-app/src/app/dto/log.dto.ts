export interface LogDto {
  type: string;
  dateTime: Date | string;
  source: string;
  description: string;
  ip: string;
}
