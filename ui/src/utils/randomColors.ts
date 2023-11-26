  const COLORS = ['#FF5733', '#33FF57', '#5733FF', '#33FFFF', '#FF33A1', '#A133FF', '#FF3333', '#33FFD1', '#FFD133', '#9B33FF', '#FFA133', '#FF33F2', '#33C7FF', '#7D33FF', '#33FF8C', '#FF8C33', '#3387FF', '#FF3370', '#3370FF'];
    
  export const getRandomColor = (value: number)  => {
    return COLORS[value % COLORS.length] 
  }