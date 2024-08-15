import { nanoid } from 'nanoid';

export type CartType = {
  id: string;
  item: CartItem[];
};

export type CartItem = {
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  brand: string;
  type: string;
};

export class Cart implements CartType {
  id = nanoid();
  item: CartItem[] = [];
}
