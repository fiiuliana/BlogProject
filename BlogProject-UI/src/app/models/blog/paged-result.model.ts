export class PagedResult<T> {

    constructor (
        public page: Array<T>,
        public totalCount: number,     
    ) 
    {

    }
}